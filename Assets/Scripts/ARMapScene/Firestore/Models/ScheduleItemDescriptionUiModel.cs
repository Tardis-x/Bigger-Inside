namespace ua.org.gdg.devfest
{
  public class ScheduleItemDescriptionUiModel
  {
    public string Title { get; set; }
    public string DateReadable { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string Hall { get; set; }
    public string Language { get; set; }
    public string Complexity { get; set; }
    public string MainTag { get; set; }
    public string[] Tags { get; set; }
    public string Description { get; set; }
    public string TagColor { get; set; }
    public Speaker[] Speakers { get; set; }
  }
}