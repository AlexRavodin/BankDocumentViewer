﻿using Viewer.Models.Options;

namespace Viewer.Services;

public interface IFileService
{
    public Task WriteStringsToFiles(FilesOptions options, GeneratingOptions generatingOptions);

    public Task<int> FilterAndConcat(FilesOptions options);

    public Task ReadAndSaveFile(FilesOptions options, GeneratingOptions generatingOptions,
        Action<int, int, int> changeProgressDelegate);
}