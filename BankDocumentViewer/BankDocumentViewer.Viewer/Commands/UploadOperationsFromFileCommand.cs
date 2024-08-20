using System.Windows;
using Microsoft.Win32;
using Viewer.Services;
using Viewer.ViewModels;

namespace Viewer.Commands;

public class UploadOperationsFromFileCommand : AsyncCommandBase
{
    private readonly FilesListViewModel _viewModel;

    private readonly IBankDocumentParser _parser;

    private readonly IDataProvider _dataProvider;

    public UploadOperationsFromFileCommand(FilesListViewModel viewModel, IBankDocumentParser parser, IDataProvider dataProvider)
    {
        _viewModel = viewModel;
        _parser = parser;
        _dataProvider = dataProvider;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        _viewModel.IsLoading = true;
        
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Файлы таблиц (*.xls)|*.xls|Все файлы (*.*)|*.*",
            Title = "Выберите файл"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            var filePath = openFileDialog.FileName;
            
            try
            {
                var parsingResultDto = _parser.Parse(filePath);

                await _dataProvider.SaveBankDocumentFile(parsingResultDto);

                var files = await _dataProvider.LoadFiles();
                
                _viewModel.UpdateFiles(files);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка загрузки файла: {e.Message}.", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                _viewModel.IsLoading = false;
            }

        }
    }
}