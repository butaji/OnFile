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

        private Event ReadEventFromPath(string path)
        {
            var fileName = new FileInfo(path).Name;
            var token = fileName.IndexOf("_");
            var json = fileName.IndexOf(".json");
            var typeName = fileName.Substring(token + 1, json - token - 1);

            var text = File.ReadAllText(path);
            var type = Assembly.Load("OnFile.Domain").GetType(typeName, true);
            var @event = (Event)JsonSerializer.DeserializeFromString(text, type);
            return @event;
        }

        public void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
        {
            _watcher.EnableRaisingEvents = false;
            
            var path = _workDir + "\\" + aggregateId + "\\" + expectedVersion;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            foreach (var @event in events)
            {
                var file = string.Format("{0}\\{1}_{2}.json", path, DateTime.Now.Ticks,
                    @event.GetType().FullName);
                SavePackage(file, @event);

                _bus.Publish(@event);
            }

            _watcher.EnableRaisingEvents = true;
        }

        private void SavePackage(string file, Event @event)
        {
            var json = JsonSerializer.SerializeToString(@event, @event.GetType());
            File.WriteAllText(file, json);
        }

        public List<Event> GetEventsForAggregate(Guid aggregateId)
        {
            var path = _workDir + "\\" + aggregateId;

            var result = new List<Event>();
            ProcessCatalog(path, result);
            return result;
        }
    }
}