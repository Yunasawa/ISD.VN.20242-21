using System.Collections.Generic;
using UnityEngine;
using YNL.JAMOS;
using YNL.Utilities.Extensions;
using YNL.Utilities.Patterns;

public class DeepLinkManager : Singleton<DeepLinkManager>
{
    public string LinkName = "productslink";
    public List<string> ValidIDs = new();
    public Dictionary<string, string> Parameters = new();

    public string DeepLinkURL { get; private set; } = "[none]";

    protected override void Awake()
    {
        base.Awake();

        Application.deepLinkActivated += OnDeepLinkActivated;
        if (!string.IsNullOrEmpty(Application.absoluteURL))
        {
            OnDeepLinkActivated(Application.absoluteURL);
        }
    }

    private void OnDeepLinkActivated(string url)
    {
        DeepLinkURL = url;
        Parameters.Clear();

        if (url.Contains(LinkName))
        {
            var id = GetIDFromURL(url);
            if (ValidIDs.Contains(id))
            {
                Main.Runtime.SelectedProduct = UID.Parse(id);
                Marker.OnViewPageSwitched?.Invoke(ViewType.InformationViewMainPage, true, true);
            }
            ExtractParametersFromURL(url);
        }
        else
        {
            MDebug.Log($"URL does not contain the expected link name: {LinkName}");
        }
    }

    private string GetIDFromURL(string url)
    {
        var uri = new System.Uri(url);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        return query.Get("id");
    }

    private void ExtractParametersFromURL(string url)
    {
        var uri = new System.Uri(url);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);

        foreach (var key in query.AllKeys)
        {
            if (key != "id")
            {
                Parameters[key] = query.Get(key);
            }
        }
    }
}