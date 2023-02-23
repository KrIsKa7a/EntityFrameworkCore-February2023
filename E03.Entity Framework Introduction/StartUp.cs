namespace SoftUni;

using System.Globalization;
using System.Text;

using Microsoft.EntityFrameworkCore;

using Data;
using Models;

public class StartUp
{
    static void Main(string[] args)
    {
        // In EF >= 3.1 even if the query is materialized the changes are saved in the DB!
        // Use AsNoTracking() instead
        SoftUniContext dbContext = new SoftUniContext();

        var employees = dbContext
            .Employees
            .Take(10)
            .AsNoTracking();
        employees.FirstOrDefault(e => e.EmployeeId == 1).FirstName = "Test";

        foreach (var employee in employees)
        {
            Console.WriteLine(employee);
        }

        dbContext.SaveChanges();

        Employee employee1 = dbContext
            .Employees
            .Find(1);
        Console.WriteLine(employee1);
    }

    // Problem 03
    public static string GetEmployeesFullInformation(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var employees = context.Employees
            .OrderBy(e => e.EmployeeId)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.MiddleName,
                e.JobTitle,
                e.Salary
            })
            .ToArray(); //ToList() -> Detach from the change tracker, any changes on the data inside will not be saved into db
        foreach (var e in employees)
        {
            sb
                .AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    // Problem 05
    public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var employeesRnD = context.Employees
            .Where(e => e.Department.Name == "Research and Development")
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                DepartmentName = e.Department.Name,
                e.Salary
            })
            .OrderBy(e => e.Salary)
            .ThenByDescending(e => e.FirstName)
            .ToArray();

        foreach (var e in employeesRnD)
        {
            sb
                .AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    // Problem 06
    public static string AddNewAddressToEmployee(SoftUniContext context)
    {
        Address newAddress = new Address()
        {
            AddressText = "Vitoshka 15",
            TownId = 4
        };
        //context.Addresses.Add(newAddress); // This is the way for adding into the db

        Employee? employee = context.Employees
            .FirstOrDefault(e => e.LastName == "Nakov");
        employee.Address = newAddress;

        context.SaveChanges();

        string[] employeeAddresses = context.Employees
            .OrderByDescending(e => e.AddressId)
            .Take(10)
            .Select(e => e.Address!.AddressText)
            .ToArray();
        return String.Join(Environment.NewLine, employeeAddresses);
    }

    // Problem 07
    public static string GetEmployeesInPeriod(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();
        var employeesWithProjects = context.Employees
            //.Where(e => e.EmployeesProjects
            //    .Any(ep => ep.Project.StartDate.Year >= 2001 &&
            //               ep.Project.StartDate.Year <= 2003))
            .Take(10)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                ManagerFirstName = e.Manager!.FirstName,
                ManagerLastName = e.Manager!.LastName,
                Projects = e.EmployeesProjects
                    .Where(ep => ep.Project.StartDate.Year >= 2001 &&
                                 ep.Project.StartDate.Year <= 2003)
                    .Select(ep => new
                    {
                        ProjectName = ep.Project.Name,
                        StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                        EndDate = ep.Project.EndDate.HasValue
                            ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                            : "not finished"
                    })
                    .ToArray()
            })
            .ToArray();

        foreach (var e in employeesWithProjects)
        {
            sb
                .AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

            foreach (var p in e.Projects)
            {
                sb
                    .AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
            }
        }

        return sb.ToString().TrimEnd();
    }

    // Problem 14
    public static string DeleteProjectById(SoftUniContext context)
    {
        // Delete all rows from EmployeeProject that refer to Project with Id = 2
        IQueryable<EmployeeProject> epToDelete = context.EmployeesProjects
            .Where(ep => ep.ProjectId == 2);
        context.EmployeesProjects.RemoveRange(epToDelete);

        Project projectToDelete = context.Projects.Find(2)!;
        context.Projects.Remove(projectToDelete);
        context.SaveChanges();

        string[] projectNames = context.Projects
            .Take(10)
            .Select(p => p.Name)
            .ToArray();
        return String.Join(Environment.NewLine, projectNames);
    }
}