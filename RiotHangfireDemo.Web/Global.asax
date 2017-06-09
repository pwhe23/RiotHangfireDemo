<%@ Application Language="C#" %>
<%@ Import Namespace="RiotHangfireDemo" %>
<script RunAt="server">
    public void Application_Start(object sender, EventArgs args)
    {
        Startup.Initialize();
    }
</script>
