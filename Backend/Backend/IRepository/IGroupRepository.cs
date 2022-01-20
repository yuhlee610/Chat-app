using Backend.DTOs;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.IRepository
{
    public interface IGroupRepository
    {
        Task<Group> CreateGroup(GroupInputDTO groupInput);
    }
}
