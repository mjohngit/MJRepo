using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelMotel.Data;
using HotelMotel.Business.Models;
using HotelMotel.Business.IFacade;

namespace HotelMotel.Business.Repository
{
    public class LoginRepository : IHmLogin
    {
        private readonly ExternalUsersMasterEntities _extUsers = new ExternalUsersMasterEntities();

        public LoginResult LogResult(string userid, string password)
        {
            var curUser = _extUsers.CheckLoginGovs(userid, password, "1", false);

            var loggedUser = curUser.Select(item =>
                    new LoginResult()
                    {
                        CicoId = item.CICOID,
                        EntityName = item.Agency,
                        AnnexLoginFips = item.FIPS,
                        Cico = item.CICO,
                        AnnexLoginResult = item.LoginResult
                    }).SingleOrDefault();

            //var loggedUser = new LoginResult { CicoId = 200002, EntityName = "Maja Agaya", AnnexLoginFips = "2992939", Cico = 2, AnnexLoginResult = "OK" };
            return loggedUser;
        }
    }
}
