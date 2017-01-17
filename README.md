# Open311 GeoReport v2 API

## Work in progress, not usable [yet].

See the [official documentation](http://wiki.open311.org/GeoReport_v2/) for more information.

This project aims to create a [ASP.NET Core](https://docs.microsoft.com/en-ca/aspnet/core/)
framework to ease implementation of the `GeoReport v2` api from [Open311](http://www.open311.org/).

## Implementation Status

The `GeoReport v2` specification defines 6 methods. We aim to implement all of them
except for [GET Service Request Id](http://wiki.open311.org/GeoReport_v2/#get-servicerequestid-from-a-token).

| API Method               | Implementation Status |
|--------------------------|-----------------------|
| Service Discovery        | planned               |
| `GET Service List`       | implemented           |
| `GET Service Definition` | implemented           |
| `POST Service Request`   | in development        |
| `GET Service Request Id` | not planned           |
| `GET Service Requests`   | implemented           |
| `GET Service Request`    | implemented           |

We currently support `xml` and `json` formats in utf-8. Please note the responses may not be fully
compliants as of today, we are still in development.

Since the storage of service requests is highly implementation specific, only interfaces are provided.
The implementation is *YOUR* responsability. It can really be any sources: web services, elastic storage,
databases, erp, etc.  To evaluate the framework, in-memory stores are provided, so you could test the
solution without any development efforts.

## Known Problems and Limitations

The current xml signature includes the default namespace (`xmlns:i="http://www.w3.org/2001/XMLSchema-instance"`).
This may be a problem for custom xml deserialization routines.

The published signatures by our implementation are not validated yet for compliance yet.
Once the code stabilize, we will make sure it is on par with the standard.

The jurisdiction is mandatory in our implementation, but it can be set to any default value
(if not provided by the caller). You should be able to make it work in any scenarios.

The `media_url` property of a service request is implementation specific. We have not evaluated
what the api should accept or not.  From the official docs:

> A convention for parsing media from this URL has yet to be established, so currently
> it will be done on a case by case basis much like Twitter.com does. For example,
> if a jurisdiction accepts photos submitted via Twitpic.com, then clients can parse the
> page at the Twitpic URL for the image given the conventions of Twitpic.com.
> This could also be a URL to a media RSS feed where the clients can parse for media
> in a more structured way.

## Contributing

We do not accept contributions for now. Once the code stabilizes, we'll revisit this decision.
