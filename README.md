# About

This is an implementation of [Ocelot](https://github.com/ThreeMammals/Ocelot) aimed at becoming containerized while
being fully customizable.

This was made primarily to interface with [Keycloak](https://github.com/keycloak/keycloak) as of yet and therefore only
include explicit functionality
for [Identity Server Bearer Tokens](https://ocelot.readthedocs.io/en/latest/features/authentication.html#identity-server-bearer-tokens)
.

## Config files

This implementation of Ocelot looks for an `ocelot.json` configuration file in two different locations, with the later
taking priority.

```
/etc/ocelot/ocelot.json
./ocelot.json
```

Ocelot's default [Configuration](https://ocelot.readthedocs.io/en/latest/features/configuration.html) has been extended
with an additional section in the root of the document. This section maps
the `Microsoft.AspNetCore.Authentication.JwtBearer` plus a new AuthenticationProviderKey field.

Below are the four fields that's mandatory for Ocelot to interface with an Identity Server using Bearer Tokens.
Additional or alternate fields from the `Microsoft.AspNetCore.Authentication.JwtBearer` can be added, but may not be
supported due to lack of implicit conversion.

```
 "JwtBearers": [
   {
     "AuthenticationProviderKey": "ProviderKey",
     "Authority": "https://auth.example.com",
     "RequireHttpsMetadata": true,
     "Audience": "audience"
   }
 ]
```

The AuthenticationProviderKey field is a key that identifies a Route with a JwtBearers rule set. Multiple rule sets can
be defined in JwtBearers, with the AuthenticationProviderKey being the identifier. Different Routes can then employ
different rules depending in their usecase.

```
"Routes": [
  {
    ...
    "AuthenticationOptions": {
      "AuthenticationProviderKey": "ProviderKey",
      ...
    }
    ...
  }
]
```