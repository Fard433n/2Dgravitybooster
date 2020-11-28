using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket_ship : MonoBehaviour
{
   
  

    

    [SerializeField]float rcsThurst = 100f;
    [SerializeField] float mainThurst = 100f;
    [SerializeField] float levelloadDelay = 2f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;
    [SerializeField] ParticleSystem mainEngineParticle;
    [SerializeField] ParticleSystem successParticle;
    [SerializeField] ParticleSystem deathParticle;

    AudioSource audiosource;
    Rigidbody rigidbody;

   
    bool collisionsDisabled = false;
    bool isTransitioning = false;

    // Start is called before the first frame update
    void Start()
    {
        

        rigidbody = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTransitioning)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        //todo only if debug on
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
       
    }

    private  void RespondToDebugKeys()
    {
       if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
       else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
        }
    }


    void OnCollisionEnter(Collision collision)

    {
        if (isTransitioning || collisionsDisabled) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly"://frndly do ntg
                break;
            case "Finish":
                startSuccessSequence();
                break;
            default:
                //SceneManager.LoadScene(0);
                startDeathSequence();
                break;
        }
    }

    private void startDeathSequence()
    {
        print("Hit something deadly");
        isTransitioning = true;
        audiosource.Stop();
        audiosource.PlayOneShot(death);
        deathParticle.Play();
        Invoke("LoadFirstLevel", levelloadDelay);
    }

    private void startSuccessSequence()
    {
        isTransitioning = true;
        audiosource.Stop();
        audiosource.PlayOneShot(success);
        successParticle.Play();
        Invoke("LoadNextLevel", levelloadDelay);
    }

    
    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
       
    }
    private void LoadNextLevel()
    {
       
        int currentsceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentsceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space)) 
        {
            ApplyThrust();

        }
        else
        {
            StopApplyingThrust();
        }

    }

    private void StopApplyingThrust()
    {
        audiosource.Stop();
        mainEngineParticle.Stop();
    }

    private void ApplyThrust()
    {
        rigidbody.AddRelativeForce(Vector3.up * mainThurst );
        if (!audiosource.isPlaying)
        {
            audiosource.PlayOneShot(mainEngine);
            
        }
        mainEngineParticle.Play();
    }

    private void RespondToRotateInput()
    {
       
     
        if (Input.GetKey(KeyCode.A))
        {
            RotateManually(rcsThurst * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateManually(-rcsThurst * Time.deltaTime);
        }
    
    }

    private void RotateManually(float rotationThisFrame)
    {
        rigidbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame);
        rigidbody.freezeRotation = false;
    }

}
