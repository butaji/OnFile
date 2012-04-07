using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using OnFile.Domain;
using ServiceStack.Text;

namespace OnFile.Storage
{
    public class DirectoryEventStore : IEventStore
    {
        private readonly IEventPublisher _bus;
        private readonly string _workDir;
        private readonly FileSystemWatcher _watcher;

        public DirectoryEventStore(IEventPublisher bus, string workDir)
        {
            _bus = bus;
            _workDir = workDir;

            _watcher = new FileSystemWatcher(workDir, "*.json");
            _watcher.IncludeSubdirectories = true;
            _watcher.Created += FileCreated;
            _watcher.EnableRaisingEvents = true;
        }

        private void FileCreated(object sender, FileSystemEventArgs e)
        {
            _bus.Publish(ReadEventFromPath(e.FullPath));
        }

        public void LoadEvents()
        {
            var events = new List<Event>();
            ProcessCatalog(_workDir, events);

            foreach (var @event in events)
            {
                _bus.Publish(@event);
            }
        }

        void ProcessCatalog(string sDir, List<Event> events)
        {
            foreach (var d in Directory.GetDirectories(sDir))
            {
                events.AddRange(Directory.GetFiles(d).Select(ReadEventFromPath));
                ProcessCatalog(d, events);
            }
        }

        public void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
        {
            _watcher.EnableRaisingEvents = false;

            var path = _workDir + "\\" + aggregateId + "\\" + expectedVersion;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            foreach (var @event in events)
            {
                var file = string.Format("{0}\\{1}.json", path, DateTime.Now.Ticks);
                SaveEvent(file, @event, aggregateId, expectedVersion);

                _bus.Publish(@event);
            }

            _watcher.EnableRaisingEvents = true;
        }

        public List<Event> GetEventsForAggregate(Guid aggregateId)
        {
            var path = _workDir + "\\" + aggregateId;

            var result = new List<Event>();
            ProcessCatalog(path, result);
            return result;
        }

        private void SaveEvent(string file, Event @event, Guid aggregateId, int expectedVersion)
        {
            SavePackage(file, new EventDescriptor(aggregateId, @event, expectedVersion));
        }

        private void SavePackage(string file, EventDescriptor package)
        {
            var json = package.ToJson();
            File.WriteAllText(file, json);
        }

        private Event ReadEventFromPath(string path)
        {
            var text = File.ReadAllText(path);
            var desr = text.FromJson<EventDescriptor>();

            return desr.EventData;
        }

        public class EventDescriptor
        {
            public Guid Id { get; set; }
            public dynamic EventData { get; set; }
            public int Version { get; set; }

            public EventDescriptor()
            {
            }

            public EventDescriptor(Guid id, Event eventData, int version)
            {
                EventData = eventData;
                Version = version;
                Id = id;
            }
        }

    }

}