using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class QuantumTunnel : Card
{
    private Player player;
    private CharacterController characterController;
    private CardReward card_reward_prefab;
    private CardEffects card_effects;
    
    // Start is called before the first frame update
    public override void Start()
    {
        Details = AssetDatabase.LoadAssetAtPath<CardDetails>("Assets/Cards/Quantum Tunnel.asset");
        player = FindAnyObjectByType<Player>();
        characterController = player.GetComponent<CharacterController>();
        card_reward_prefab = AssetDatabase.LoadAssetAtPath<CardReward>("Assets/Card Reward.prefab");
        card_effects = FindAnyObjectByType<CardEffects>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForCollisions();
    }

    private bool found_collider;
    private Vector3 center_of_collider_direction;
    private Collider[] colliders_buffer = new Collider[10];
    private Vector3 proposed_destination_for_player;

    public override void PlayCard()
    {
        int number_of_colliders = CheckForCollisions();
        if (number_of_colliders > 0) {
            Vector3 center_of_collider = colliders_buffer[0].bounds.center;
            Vector3 towards_center_of_collider = center_of_collider - player.transform.position;
            towards_center_of_collider.y = 0.0f;
            towards_center_of_collider.Normalize();
            center_of_collider_direction = towards_center_of_collider * 5.0f;
            Vector3 test_position = player.transform.position + towards_center_of_collider * 5.0f;
            float point_to_position_length = characterController.height / 2 - characterController.radius;
            bool capsule_cast_hit = Physics.CapsuleCast(test_position - Vector3.up * point_to_position_length,
                                                        test_position + Vector3.up * point_to_position_length,
                                                        characterController.radius,
                                                        -towards_center_of_collider,
                                                        out RaycastHit hit_info,
                                                        5.0f);
            if (capsule_cast_hit && hit_info.distance > 0.0f) {
                proposed_destination_for_player = test_position - towards_center_of_collider * (hit_info.distance - 0.2f);
                Vector3 old_player_location = player.transform.position;
                characterController.enabled = false;
                player.transform.position = proposed_destination_for_player;
                characterController.enabled = true;
                CardReward leave_behind = Instantiate(card_reward_prefab);
                leave_behind.transform.position = old_player_location - Vector3.up * 0.9f;
                leave_behind.SetCard(this);
                leave_behind.transform.rotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
                card_effects.RemoveCard(this);
            }
        } else {
            center_of_collider_direction = Vector3.zero;
        }
    }

    void OnDrawGizmos() {
        Gizmos.matrix = player.transform.localToWorldMatrix;
        Vector3 box_center = Vector3.forward * 2.5f;
        Vector3 half_box_size = new Vector3(1.0f, 0.8f, 2.0f);
        Gizmos.color = found_collider ? Color.red : Color.yellow;
        Gizmos.DrawWireCube(box_center, half_box_size * 2);
        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.DrawLine(player.transform.position, player.transform.position + center_of_collider_direction);
        if (Physics.OverlapCapsuleNonAlloc(proposed_destination_for_player - Vector3.up * 0.5f, proposed_destination_for_player + Vector3.up * 0.5f, 0.5f, new Collider[1]) > 0) {
            Gizmos.color = Color.red;
        } else {
            Gizmos.color = Color.yellow;
        }
        Gizmos.DrawWireSphere(proposed_destination_for_player - Vector3.up * 0.5f, 0.5f);
        Gizmos.DrawWireSphere(proposed_destination_for_player + Vector3.up * 0.5f, 0.5f);
    }

    private int CheckForCollisions() {
        Vector3 box_center = player.transform.position + player.transform.forward * 2.5f;
        Vector3 half_box_size = new Vector3(1.0f, 0.8f, 2.0f);
        int number_of_colliders = Physics.OverlapBoxNonAlloc(box_center, half_box_size, colliders_buffer, player.transform.rotation);
        found_collider = number_of_colliders > 0;
        return number_of_colliders;
    }
}
