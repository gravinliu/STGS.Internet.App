namespace STGS.Internet.Core
{
    public class ReturnValue
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string ErroMessage { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public object ReturnObject { get; set; }
    }
}
