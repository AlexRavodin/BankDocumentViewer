namespace Viewer.Models.Dto;

public class OperationClassDto
{
    public int Number { get; set; }
    
    public string Name { get; set; }

    public List<OperationDto> Operations { get; set; } = [];
}