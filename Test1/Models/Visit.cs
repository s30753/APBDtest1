namespace Test1.Models;

public class Visit
{
    public int visit_id { get; set; }
    public Client client { get; set; }
    public Mechanic mechanic { get; set; }
    public DateTime date { get; set; }
    public List<Visit_Service> visit_services { get; set; }
}