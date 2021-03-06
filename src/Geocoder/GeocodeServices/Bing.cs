﻿using System.Diagnostics;
using System.Threading.Tasks;
using NLog;
using RestSharp;

namespace Geo.Coder.GeocodeServices
{
  public class BingGeoCoder : IGeocode
  {
    private readonly Logger _log;

    public BingGeoCoder()
    {
      _log = LogManager.GetLogger(nameof(BingGeoCoder));
    }

    public string BaseUrl => @"http://dev.virtualearth.net";

    public string GeoCodeRestUrl => @"REST/v1/Locations";

    public async Task<string> Find(ApiGeocodeQuery queryParams)
    {
      var watch = Stopwatch.StartNew();
      var client = new RestClient(BaseUrl);
      var request = new RestRequest(GeoCodeRestUrl);

      request.AddObject(queryParams);

      _log.Debug("Request constructed '{0}' {1} milliseconds", client.BuildUri(request), watch.ElapsedMilliseconds);
      var geoResponse = await client.ExecuteAsGet(request, HttpMethods.Get).GetTaskResult();
      _log.Debug("Geocode response: {0}", geoResponse.Content);

      return geoResponse.Content;
    }
  }
}