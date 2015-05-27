using System;

namespace ParatureSDK.ParaObjects
{
    public class View : ParaEntityBaseProperties
    {
        public string Name { get; set; }

        public View()
        {
            Name = "";
            Id = 0;
        }

        public View(View view)
        {
            Id = view.Id;
            Name = view.Name;
            FullyLoaded = view.FullyLoaded;
            ApiCallResponse = new ApiCallResponse(view.ApiCallResponse);
        }

    }
}