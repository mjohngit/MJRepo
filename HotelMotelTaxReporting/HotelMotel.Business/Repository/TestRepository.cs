using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using HotelMotel.Data;
using HotelMotel.Business.IFacade;
using HotelMotel.Business.Models;
namespace HotelMotel.Business.Repository
{
    public class TestRepository 
    {
        public string GetStringResult(string uid, string pwd)
        {
            var myStr = "My uid is " + uid + " and pwd is " + pwd;
            return (myStr);
        }

        //public List<MjTestClass>  GetVal()
        //{
        //    var mjTest = new MjTestClass( {SId = 3, Fname = "Mathew", LName = "John"};
        //    mjTest += new MjTestClass({SId = 4,Fname = "Sundar", LName = "Pichai"});
        //    var mtest = new MjTestClass();

        //    foreach (var item in mjTest)
        //    {
        //        mtest = mjTest;
        //    }

        //    var varResults = mtest.Select(Mapper.DynamicMap<MjTestClass>).SingleOrDefault();
        //}
    }

    public class MjTestClass
    {
        public int SId { get; set; }
        public string Fname { get; set; }
        public string LName { get; set; }
    }
}
