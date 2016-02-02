#ifndef AUGMENTA_STANDARD_TEMPERATURE_INCLUDED
#define AUGMENTA_STANDARD_TEMPERATURE_INCLUDED

#define TWOTO32 4294967296.0
#define TWOTO24 16777216.0
#define TWOTO16 65536.0
#define TWOTO8 256.0

#define MAX32BIT 4294967295.0
#define MAX24BIT 16777215.0
#define MAX16BIT 65535.0
#define MAX8BIT 255.0

half3 TemperatureHighSM4(sampler2D lookup, half uvCoord)
{
	uint4 tempRedTex = abs(tex2D(lookup, half2(uvCoord,0.944))*MAX8BIT);
	uint4 tempGreenTex = abs(tex2D(lookup, half2(uvCoord,0.833))*MAX8BIT);
	uint4 tempBlueTex = abs(tex2D(lookup, half2(uvCoord,0.722))*MAX8BIT);
	uint4 tempIntensityTex = abs(tex2D(lookup, half2(uvCoord,0.611))*MAX8BIT);

	uint tempIntensityDecoded = (tempIntensityTex.x<<24) | (tempIntensityTex.y<<16) | (tempIntensityTex.z<<8) | (tempIntensityTex.a);
	uint tempRedDecoded = (tempRedTex.x<<24) | (tempRedTex.y<<16) | (tempRedTex.z<<8) | (tempRedTex.a);
	uint tempGreenDecoded = (tempGreenTex.x<<24) | (tempGreenTex.y<<16) | (tempGreenTex.z<<8) | (tempGreenTex.a);
	uint tempBlueDecoded = (tempBlueTex.x<<24) | (tempBlueTex.y<<16) | (tempBlueTex.z<<8) | (tempBlueTex.a);

	float tempIntensityConverted = asfloat(tempIntensityDecoded);
	float tempRedConverted = asfloat(tempRedDecoded);
	float tempGreenConverted = asfloat(tempGreenDecoded);
	float tempBlueConverted = asfloat(tempBlueDecoded);

	return (half3(tempRedConverted,tempGreenConverted,tempBlueConverted)+half3(0.005,0.005,0.005))*tempIntensityConverted;
}

half3 TemperatureHighSM3(sampler2D lookup, half uvCoord)
{
	uint4 tempRedTex = abs(tex2D(lookup, half2(uvCoord,0.500))*MAX8BIT);
	uint4 tempGreenTex = abs(tex2D(lookup, half2(uvCoord,0.389))*MAX8BIT);
	uint4 tempBlueTex = abs(tex2D(lookup, half2(uvCoord,0.278))*MAX8BIT);
	uint4 tempIntensityTex = abs(tex2D(lookup, half2(uvCoord,0.167))*MAX8BIT);

	uint tempIntensityDecoded = (tempIntensityTex.x*TWOTO24) + (tempIntensityTex.y*TWOTO16) + (tempIntensityTex.z*TWOTO8) + (tempIntensityTex.a);
	uint tempRedDecoded = (tempRedTex.x*TWOTO24) + (tempRedTex.y*TWOTO16) + (tempRedTex.z*TWOTO8) + (tempRedTex.a);
	uint tempGreenDecoded = (tempGreenTex.x*TWOTO24) + (tempGreenTex.y*TWOTO16) + (tempGreenTex.z*TWOTO8) + (tempGreenTex.a);
	uint tempBlueDecoded = (tempBlueTex.x*TWOTO24) + (tempBlueTex.y*TWOTO16) + (tempBlueTex.z*TWOTO8) + (tempBlueTex.a);

	float tempIntensityConverted = tempIntensityDecoded/TWOTO16;
	float tempRedConverted = tempRedDecoded/MAX32BIT;
	float tempGreenConverted = tempGreenDecoded/MAX32BIT;
	float tempBlueConverted = tempBlueDecoded/MAX32BIT;

	return (half3(tempRedConverted,tempGreenConverted,tempBlueConverted)+half3(0.005,0.005,0.005))*tempIntensityConverted;
}

half3 TemperatureLowSM4(sampler2D lookup, half uvCoord)
{
	half3 tempColourTex = tex2D(lookup, half2(uvCoord,0.056));
	uint4 tempIntensityTex = tex2D(lookup, half2(uvCoord,0.611))*MAX8BIT;

	uint tempIntensityDecoded = (tempIntensityTex.x<<24) | (tempIntensityTex.y<<16) | (tempIntensityTex.z<<8) | (tempIntensityTex.a);

	float tempIntensityConverted = asfloat(tempIntensityDecoded);

	return (half3(tempColourTex.rgb)+half3(0.005,0.005,0.005))*tempIntensityConverted;
}

half3 TemperatureLowSM3(sampler2D lookup, half uvCoord)
{
	half4 tempColourTex = tex2D(lookup, half2(uvCoord,0.056));
	uint4 tempIntensityTex = abs(tex2D(lookup, half2(uvCoord,0.167))*MAX8BIT);

	uint tempIntensityDecoded = (tempIntensityTex.x*TWOTO24) + (tempIntensityTex.y*TWOTO16) + (tempIntensityTex.z*TWOTO8) + (tempIntensityTex.a);

	float tempIntensityConverted = tempIntensityDecoded/TWOTO8;

	return (half3(tempColourTex.rgb)+half3(0.005,0.005,0.005))*tempIntensityConverted;
}

#endif	// AUGMENTA_STANDARD_TEMPERATURE_INCLUDED