using UnityEngine;

public class AudioManager : MonoBehaviour {


    public AudioSource button;
    public AudioSource winner;
    public AudioSource waiting;
    public AudioSource starting;
    public AudioSource game;





    public void playButtton()
    {
        stop(starting);
        stop(game);
        play(button);
    }
    
    public void playWinner()
    {
        play(winner);
        
    }
    public void playGame()
    {
        stop(starting);
        play(game);
        
    }
    
    public void playWaiting()
    {
        play(waiting);
        
    }


    public void play(AudioSource audioSource)
    {
        audioSource.Play();
    }
    
    public void stop(AudioSource audioSource)
    {
        audioSource.Stop();
    }


}