using System;
using System.Collections.Generic;
using System.IO;
using DAL.History.Dto;
using Core.Dal.Interfaces;
using Core.Manager.File.Interfaces;
using Core.Model.History;
using DAL.Extensions;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace DAL.History
{
    public class HistoryRepository : IHistoryRepository
    {
        public List<HistoryObjectModel> GetModels(string path)
        {
            if (!File.Exists(path))
                return new List<HistoryObjectModel>();

            var files = Directory.GetFiles(path);

            List<FileActionDto> objectOut = new List<FileActionDto>();
            foreach (var filePath in files)
            {
                var rawJson = File.ReadAllText(filePath);
                var readedList = JsonConvert.DeserializeObject<List<FileActionDto>>(rawJson);
                objectOut.AddRange(readedList);
            }

            var result = new List<HistoryObjectModel>();

            foreach (var fileActionDto in objectOut)
            {
                var historyModel = new HistoryObjectModel(fileActionDto.FileName,
                    fileActionDto.OldFolder,
                    fileActionDto.NewFolder)
                {
                    FileActions = fileActionDto.FileActions.Convert(), DateTime = fileActionDto.DateTime
                };

                result.Add(historyModel);
            }

            return result;
        }

        public void SaveNewEvents(string filePath, IList<HistoryObjectModel> obj)
        {
            if (obj == null || obj.Count == 0)
                return;

            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(Path.Combine(filePath, DateTime.Now.ToString("yyyy MMMM dd")), json);
        }
    }
}