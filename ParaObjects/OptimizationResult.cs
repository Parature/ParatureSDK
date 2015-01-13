namespace ParatureAPI.ParaObjects
{
    public class OptimizationResult
    {
        public ParaQuery Query;

        public PagedData.PagedData objectList;

        public ApiCallResponse apiResponse;

        public OptimizationResult()
        {
        }

        public OptimizationResult(OptimizationResult optResult)
        {
            Query = optResult.Query;
            objectList = optResult.objectList;
            apiResponse = new ApiCallResponse(optResult.apiResponse);
        }
    }
}