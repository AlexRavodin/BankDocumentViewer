using System.Windows;
using BankDocumentViewer.Viewer.Services;
using BankDocumentViewer.Viewer.ViewModels;

namespace BankDocumentViewer.Viewer.Commands;

public class ShowStatisticsCommand : AsyncCommandBase
{
    private readonly GenerateViewModel _viewModel;

    private readonly IDataProvider _dataProvider;

    public ShowStatisticsCommand(GenerateViewModel viewModel, IDataProvider dataProvider)
    {
        _viewModel = viewModel;
        _dataProvider = dataProvider;
    }

    public override async Task  ExecuteAsync(object parameter)
    {
        _viewModel.IsStatisticsLoading = true;
        try
        {
            await Task.Delay(1000);
            
            var stats = await _dataProvider.CalculateSumAndMedian();
            
            MessageBox.Show($"Сумма всех целых чисел: {stats.IntegerSum}" + Environment.NewLine +
                            $"Медиана дробных чисел: {stats.FloatMedian.ToString("F2")}", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception e)
        {
            MessageBox.Show($"Ошибка получения статистики: {e.Message}.", "Ошибка", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        finally
        {
            _viewModel.IsStatisticsLoading = false;
        }
    }
}