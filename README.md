### Business Day Counter

##### Request Example

```
curl -X GET "https://localhost:5001/calculate?startDate=12-12-2014&endDate=11-11-2015" -H "accept: text/plain"
```

##### Response Example

```
{
  "numberOfBusinessDays": 10
}
```

## Implementation Details

- ASP.NET Core 3.0
- Serilog
- Swagger Doc
- Unit tests Completed ( xUnit )
- Integration Tests ( Not Completed )