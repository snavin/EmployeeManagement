using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;

        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee() {  ID=1,Name="Mary",Email="mary@gmail.com",Department=Dept.HR},
                new Employee() {  ID=2,Name="John",Email="john@gmail.com",Department=Dept.IT},
                new Employee() {  ID=3,Name="Sam",Email="sam@gmail.com",Department=Dept.Payroll}
            };
        }

        public Employee GetEmployee(int Id)
        {
            return _employeeList.FirstOrDefault(x => x.ID == Id);
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee Add(Employee employee)
        {
            employee.ID=_employeeList.Max(x => x.ID) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee employee =_employeeList.FirstOrDefault(x => x.ID ==employeeChanges.ID);
            if (employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;

            }
            return employee;

        }

        public Employee Delete(int id)
        {
            Employee employee=_employeeList.FirstOrDefault(m => m.ID == id);
            if(employee!=null)
            {
                _employeeList.Remove(employee);
            }
            return employee;
        }
    }
}
