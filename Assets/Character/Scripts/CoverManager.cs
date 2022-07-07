using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverManager : MonoBehaviour
{
    public GameObject playerObject;
    private TopDownCharacterMover topDownCharacterMover;

    private void Awake() {
        topDownCharacterMover = playerObject.GetComponent<TopDownCharacterMover>();
    }

    private void OnTriggerEnter(Collider other) {
        topDownCharacterMover.PlayerCoverIn();
    }
    private void OnTriggerExit(Collider other) {
        topDownCharacterMover.PlayerCoverOut();
    }
}
