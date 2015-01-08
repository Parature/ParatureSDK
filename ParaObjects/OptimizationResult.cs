namespace ParatureAPI.ParaObjects
{
    public partial class OptimizationResult
    {
        public ParaQuery Query;

        public PagedData.PagedData objectList;

        public ApiCallResponse apiResponse;

        public OptimizationResult()
        {
        }

        public OptimizationResult(OptimizationResult optResult)
        {
            this.Query = optResult.Query;
            this.objectList = optResult.objectList;
            this.apiResponse = new ApiCallResponse(optResult.apiResponse);
        }
    }
}