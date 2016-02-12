namespace ParatureSDK.Fields
{
    public class StaticField : Field
    {
        public string SchemaTag;
        internal bool IgnoreSerializeXml = false; //some static fields come from the server, but shouldn't ever be sent back
    }
}
