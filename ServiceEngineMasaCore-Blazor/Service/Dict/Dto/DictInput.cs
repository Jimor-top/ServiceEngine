using Furion.DataValidation;
using ServiceEngine.Core;

namespace ServiceEngineMasaCore.Blazor.Service.Dict.Dto
{
    public class DictInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public virtual long Id { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public StatusEnum Status { get; set; }
    }
}
