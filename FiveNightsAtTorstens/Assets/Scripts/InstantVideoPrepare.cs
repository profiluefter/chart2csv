using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class InstantVideoPrepare : MonoBehaviour
{
    private void Start()
    {
        var player = GetComponent<VideoPlayer>();
        player.Prepare();
        player.loopPointReached += source => source.Pause();
    }
}
