using Microsoft.Extensions.DependencyInjection;
using ReXLPgDA.Repos;
using ReXLPgDM;
using System;

namespace ReXLPgDA
{
    public interface IEnquiryDA
    {
        public void Add(Enquiry enquiry);
        public void Update(Enquiry enquiry);
        public Enquiry Get(Guid enquiryId);
        public List<Enquiry> GetAll();
    }
    public class EnquiryDA : IEnquiryDA
    {
        private readonly IUnitOfWork _uW;
        public EnquiryDA(IServiceProvider serviceProvider)
        {
            _uW = serviceProvider.GetService<IUnitOfWork>();
        }

        public void Add(Enquiry enquiry)
        {
            enquiry.Status = EnquiryStatus.Open;
            _uW.EnquiryTBL.Value.Insert(enquiry);
        }
        public void Update(Enquiry enquiry) => _uW.EnquiryTBL.Value.Update(_uW.EnquiryTBL.Value.GetById(enquiry.GUID), enquiry);
        public Enquiry Get(Guid enquiryId) => _uW.EnquiryTBL.Value.GetById(enquiryId);
        public List<Enquiry> GetAll() => _uW.EnquiryTBL.Value.GetAll().ToList();
    }
}
