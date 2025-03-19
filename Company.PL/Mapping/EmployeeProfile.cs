﻿using AutoMapper;
using Company.DAL.Models;
using Company.PL.Dtos;

namespace Company.PL.Mapping
{
    // CLR
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<Employee, CreateEmployeeDto>();
        }
    }
}
