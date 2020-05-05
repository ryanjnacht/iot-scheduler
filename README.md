# iot-scheduler
scheduling service for [iot-api](https://github.com/ryanjnacht/iot-api)

## Environment Variables
- MONGO_HOST (required)
- IOT_API (required)
- MONGO_DB
- MONGO_PORT

## API
GET {url}/schedules

GET {url}/status

POST {url}/schedules:
- start_time
- duration (seconds)
- devices
  - deviceId (from iot-api)
  - accessKey (from iot-api)
  - start_action (on/off)
  - end_action (on/off)
  
```
{
    "start_time": "17:34:30",
    "duration": 20,
    "devices": [
        {
            "deviceId": "bedroom-lamp",
            "accessKey": "8da8908f9540459d911a800ec902ae2d",
            "start_action": "on",
            "end_action": "off"
        }
    ]
}
```
