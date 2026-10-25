using UnityEngine;
using System.Collections;

public class VRAutomaticDoor : MonoBehaviour
{
    [Header("Komponen Pintu")]
    [Tooltip("Masukkan objek 3D pintu kiri")]
    public Transform leftDoor;
    [Tooltip("Masukkan objek 3D pintu kanan")]
    public Transform rightDoor;

    [Header("Pengaturan Pintu")]
    [Tooltip("Seberapa jauh pintu terbuka dari posisi awal")]
    public float openDistance = 1.2f;
    [Tooltip("Kecepatan gerak pintu (semakin besar semakin cepat)")]
    public float openSpeed = 2f;
    
    [Tooltip("Arah buka pintu kiri (X, Y, Z). Biasanya X = -1 atau 1")]
    public Vector3 leftOpenDirection = new Vector3(-1, 0, 0);
    [Tooltip("Arah buka pintu kanan (X, Y, Z). Biasanya kebalikan pintu kiri")]
    public Vector3 rightOpenDirection = new Vector3(1, 0, 0);

    private Vector3 leftClosedPos;
    private Vector3 rightClosedPos;
    private Vector3 leftOpenPos;
    private Vector3 rightOpenPos;

    private Coroutine moveCoroutine;

    void Start()
    {
        // Simpan posisi awal (saat tertutup/menempel)
        leftClosedPos = leftDoor.localPosition;
        rightClosedPos = rightDoor.localPosition;

        // Hitung posisi saat terbuka penuh
        leftOpenPos = leftClosedPos + (leftOpenDirection.normalized * openDistance);
        rightOpenPos = rightClosedPos + (rightOpenDirection.normalized * openDistance);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Jika player masuk ke trigger, buka pintu
        if (other.CompareTag("Player"))
        {
            if (moveCoroutine != null) StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveDoors(leftOpenPos, rightOpenPos));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Jika player keluar dari trigger, tutup pintu
        if (other.CompareTag("Player"))
        {
            if (moveCoroutine != null) StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveDoors(leftClosedPos, rightClosedPos));
        }
    }

    private IEnumerator MoveDoors(Vector3 targetLeft, Vector3 targetRight)
    {
        // Menggunakan MoveTowards agar pergerakan konstan dan TIDAK kebablasan (overshoot)
        while (leftDoor.localPosition != targetLeft || rightDoor.localPosition != targetRight)
        {
            leftDoor.localPosition = Vector3.MoveTowards(leftDoor.localPosition, targetLeft, Time.deltaTime * openSpeed);
            rightDoor.localPosition = Vector3.MoveTowards(rightDoor.localPosition, targetRight, Time.deltaTime * openSpeed);
            
            yield return null;
        }
    }
}