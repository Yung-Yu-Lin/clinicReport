namespace ClinicApplication.Models
{
	public class ClinicHomeViewModel
	{
		public Customer customer { get; set; }
		public List<TestDOC> TestDOCList { get; set; }
		public string connectionName { get; set; }
	}
}
