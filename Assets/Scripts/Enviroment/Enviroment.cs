using UnityEngine;
public class Enviroment : MonoBehaviour
{
    public EnviromentManagement env;
    public EnviromentManagement.Enviroments enterEnviroment = EnviromentManagement.Enviroments.None;
    public EnviromentManagement.Enviroments exitEnviroment = EnviromentManagement.Enviroments.None;

    private bool isThis;
    void Start()
    {
        if (enterEnviroment == EnviromentManagement.Enviroments.None)
        {
            Debug.Log("enterEnviroment sin setear." + gameObject);
        }
        if (exitEnviroment == EnviromentManagement.Enviroments.None)
        {
            Debug.Log("exitEnviroment sin setear." + gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Global.LayerEquals(env.player, other.gameObject.layer))
        {
            for (int i = 0; i < env.enviroments.Count; i++)
            {
                env.enviroments[i].isThis = false;
            }
            isThis = true;

            if (env.enviroments.Count > 0)
            {
                if (enterEnviroment != env.enviroments[0].enterEnviroment)
                {
                    Debug.Log("Cambio de Música a " + env.test[(int) enterEnviroment]);
                    //ChangeMusicAmbient(env.enviroments[0].enterEnviroment);
                }
            }
            else
            {
                Debug.Log("Cambio de Música a " + env.test[(int)enterEnviroment]);
                //ChangeMusicAmbient(env.enviroments[0].enterEnviroment);
            }

            env.enviroments.Insert(0, this);

            Debug.Log("Player entró.");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (Global.LayerEquals(env.player, other.gameObject.layer))
        {
            env.enviroments.Remove(this);
            if (isThis)
            {
                isThis = false;
                if (env.enviroments.Count > 0)
                {
                    Debug.Log("Cambio de Música a " + env.test[(int) env.enviroments[0].enterEnviroment]);
                    //ChangeMusicAmbient(env.enviroments[0].enterEnviroment);
                }
                else
                {
                    Debug.Log("Cambio de Música a " + env.test[(int) exitEnviroment]);
                    //ChangeMusicAmbient(env.enviroments[0].exitEnviroment);
                }
            }
        }
    }
    public void ChangeMusicAmbient(EnviromentManagement.Enviroments currentEnviroment)
    {
        switch (currentEnviroment)
        {
            case EnviromentManagement.Enviroments.None:
                break;
            case EnviromentManagement.Enviroments.Beach:
                AkSoundEngine.PostEvent(AK.EVENTS.AMBBEACH, gameObject);
                break;
            case EnviromentManagement.Enviroments.Field:
                AkSoundEngine.PostEvent(AK.EVENTS.AMBFIELD, gameObject);
                break;
            case EnviromentManagement.Enviroments.Forest:
                AkSoundEngine.PostEvent(AK.EVENTS.AMBFOREST, gameObject);
                break;
            case EnviromentManagement.Enviroments.Boss:
                AkSoundEngine.PostEvent(AK.EVENTS.MUSICBOSSON, gameObject);
                break;
            default:
                Debug.LogWarning("Audio No Implementado.");
                break;
        }
    }
}