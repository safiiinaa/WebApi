using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiController : ControllerBase
    {
        // GET: api/<ValuesController1>
        [HttpGet]
        public string Get()
        {
            return "Hello from WebApi";
        }

        // POST api/<ValuesController1>
        [HttpPost("calculate_salary_sum")]
        public IActionResult Post([FromBody] dynamic json)
        {
            var list = JsonConvert.DeserializeObject<List<User>>(json.ToString());
           
            var resultSum = 0.0;

            dynamic employee = default;
            foreach (var userProperty in list)
            {
                switch(userProperty.Position)
                {
                    case "manager":
                        {
                            employee = new Manager(userProperty);
                            break;
                        }
                    case "technician":
                        {
                            employee = new Technician(userProperty);
                            break;
                        }
                    case "driver":
                        {
                            employee = new Driver(userProperty);
                            break;
                        }
                    
                }
                employee.CalcSalary();
                resultSum += employee.GetSalary();
            }

            return  Ok(resultSum);
        }

    }
    public class User
    {
        public string Position { get; set; }
        public double Salary { get; set; }
        public double Bonus { get; set; }
        public double TimeWorked { get; set; }
        public string Category { get; set; }

    }
    //Главный класс - сотрудник компании
    public class Employee
    {
        public  User _user { get; set; }

        public Employee(User user)
        {
            _user = user;
        }

        //Применения категории к зарплате
        public void UseCategory()
        {
            switch(_user.Category)
            {
                case "A":
                    {
                        _user.Salary *= 1.25;
                        break; 
                    }
                case "B":
                    {
                        _user.Salary *= 1.15;
                        break;
                    }
            }
        }

        //Метод подсчета зарплаты при почасовой ставке
        public void UseTimeWorked()
        {
            _user.Salary *= _user.TimeWorked;
        }

        //Метод применения бонуса к зарплате
        public void UseBonus()
        {
            _user.Salary += _user.Bonus;
        }

        public double? GetSalary()
        {
            return _user.Salary;
        }
    }


    //Подкласс - Менеджер
    public class Manager : Employee
    {
        public Manager(User user):base(user)
        {
            _user = user;
        }

        public void CalcSalary()
        {
            UseBonus();
        }

    }
    //Подкласс - Технический специалист
    public class Technician : Employee
    {
        public Technician(User user) : base(user)
        {
            _user = user;
        }

        public void CalcSalary()
        {
            UseCategory();
            UseBonus();
        }

    }
    //Подкласс - Водитель
    public class Driver : Employee
    {
        public Driver(User user) : base(user)
        {
            _user = user;
        }

        public void CalcSalary()
        {
            UseTimeWorked();
            UseCategory();
            UseBonus();
        }
    }

}
