using System;
using UnityEngine;
using System.Collections;

public class SmoothFollow2D : MonoBehaviour
{
    /// <summary>
    /// Maximum camera zoom;
    /// </summary>
    private const ushort MaximumCameraZoom = 2;

    /// <summary>
    /// Step per zoom level.
    /// </summary>
    private const float ZoomStep = 5.0f;

    /// <summary>
    /// Instance of the camera component.
    /// </summary>
    private Camera m_camera;

    /// <summary>
    /// Velocity of the camera at this moment.
    /// </summary>
    private Vector2 m_velocity;

    /// <summary>
    /// Camera zoom.
    /// </summary>
    private ushort m_zoom = 0;

    /// <summary>
    /// Velocity of the zoom.
    /// </summary>
    private float m_zoomVelocity = 0.0f;

    /// <summary>
    /// Time for the smoothing.
    /// </summary>
    public float m_smoothTime = 0.3f;

    /// <summary>
    /// Target to follow.
    /// </summary>
    public Transform m_target;

    /// <summary>
    /// Time for the camera transition.
    /// </summary>
    public float m_zoomTransitionTime = 1.0f;

    /// <summary>
    /// Called when the script is created.
    /// </summary>
    private void Awake()
    {
        m_camera = GetComponent<Camera>();
    }

    /// <summary>
    /// Updates the camera position.
    /// </summary>
    private void Update()
    {
        if (m_target == null)
            return;

        var transformX = Mathf.SmoothDamp(transform.position.x, m_target.position.x, ref m_velocity.x, m_smoothTime);
        var transformY = Mathf.SmoothDamp(transform.position.y, m_target.position.y, ref m_velocity.y, m_smoothTime);

        transform.position = new Vector3(transformX, transformY, transform.position.z);

        m_camera.orthographicSize = Mathf.SmoothDamp(m_camera.orthographicSize, (m_zoom + 1) * ZoomStep, ref m_zoomVelocity, m_zoomTransitionTime);
    }

    /// <summary>
    /// Zooms the camera in.
    /// </summary>
    public void ZoomIn()
    {
        if (m_zoom == 0)
            return;

        --m_zoom;
    }

    /// <summary>
    /// Zooms the camera out.
    /// </summary>
    public void ZoomOut()
    {
        if (m_zoom >= MaximumCameraZoom)
            return;

        ++m_zoom;
    }
}