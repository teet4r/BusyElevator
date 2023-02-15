using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Delivery : Human, ActInterface
{
    [SerializeField]
    SpriteRenderer item_sprite;
    [SerializeField]
    Sprite[] item_sprites;

    public static class DeliveryItem
    {
        public const int INDEX_BEGIN = 0;

        public const int WATERBUCKET     = 0;
        public const int BEVERAGESBOXES  = 1;
        public const int DELIVERYBOXES   = 2;
        public const int AD              = 3;   

        public const int INDEX_END = 3;
    }

    int event_object_kind, event_object_id;
    public void SetItem(int event_object_kind, int event_object_id,int need_item,Floor destination_floor_script) // 가야할 이벤트오브젝트 미리 받기(Set보다 일찍)
    {
        this.event_object_kind = event_object_kind;
        this.event_object_id = event_object_id;
        item_sprite.sprite = item_sprites[need_item];
        destinationfloor_script = destination_floor_script;
        destination_floor = destinationfloor_script.floor_level;
    }
    public override void Set(Floor currentfloor_script)
    {
        color = PID.DELIVERY;
        dirty_size = Random.Range(6f, 10f);
        speed = Random.Range(0.5f, 1) * (1 + Gamemanager.Instance.buffmanager.delivery_speed_rate);
        exp = 10;
        gold = 100;
        population = 3;
        animator.speed = speed / 3;

        is_INF_human = true;

        item_sprite.gameObject.SetActive(true);

        spriteRenderer.flipX = false;

        this.currentfloor_script = currentfloor_script;

        speech_bubble.gameObject.SetActive(true);
        speech_bubble.sprite = Gamemanager.Instance.buildgame.bubble[Bubble.EMPTY];
        des_bubble_text.text = destination_floor.ToString();

        item_sprite.sortingLayerName = "HumanAfter";
        item_sprite.sortingOrder = 1;
        spriteRenderer.sortingLayerName = "HumanAfter";
        speech_bubble.sortingLayerName = "HumanAfter";

        if (destination_floor > currentfloor_script.floor_level)
        {
            gameObject.layer = Mask.Up;
            StartMove(currentfloor_script.spawn_point_up, true);
        }
        else
        {
            gameObject.layer = Mask.Down;
            StartMove(currentfloor_script.spawn_point_down, true);
        }
    }
    public override void ChangeSpriteLayer(string layername)
    {
        base.ChangeSpriteLayer(layername);
        item_sprite.sortingLayerName = layername;
    }
    public override IEnumerator ActAtVM(VendingMachine vm)
    {
        item_sprite.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        vm.delivery_called = false;
        vm.use_count += 15;
    }
    public override IEnumerator ActAtWP(WaterPurifier wp)
    {
        item_sprite.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        wp.delivery_called = false;
        wp.use_count += 15;
    }
    protected override IEnumerator MoveRight(Vector2 start)
    {
        animator.SetBool("isWaiting", false);
        points.Clear();
        rigid.position = start;
        // 방문할 지점 받아올 때까지 기다림
        yield return destinationfloor_script.StartCoroutine(destinationfloor_script.GetSpecificationEventPoints(points, event_object_kind, event_object_id));

        // 지점 방문
        while (points.Count != 0)
        {
            EventObject point = points.Dequeue();
            Vector2 point_pos = point.gameObject.transform.position;
            transform.rotation = Quaternion.Euler(new Vector3(0, (rigid.position.x < point_pos.x) ? -180 : 0, 0));  // 이동하려는 방향으로 돌아섬
            while (rigid.position != point_pos)
            {
                rigid.position = Vector2.MoveTowards(rigid.position, point_pos, speed * Time.deltaTime);
                yield return null;
            }
            yield return StartCoroutine(point.EventOn(this));
        }
    }
}
