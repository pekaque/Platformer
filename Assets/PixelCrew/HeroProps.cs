
using System;
using UnityEngine;

namespace PixelCrew
{
    public class HeroProps : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;

        [SerializeField] private LayerCheck _groundCheck;

        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        private Animator _animator;
        private SpriteRenderer _sprite;
        private bool _isGrounded;
        private bool _isSecondJumpAllowed;
        
        private static readonly int IsGroundKey = Animator.StringToHash("is-grounded");
        private static readonly int IsRunning= Animator.StringToHash("is-running");
        private static readonly int VerticalVelocityKey = Animator.StringToHash("vertical-velocity");
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        private void Update()
        {
            _isGrounded = IsGrounded();
        }
        
        private void FixedUpdate()
        {
            var xVelocity = _direction.x * _speed; //скорость приложенная к физ телу по оси Х равна направлению * на заданную скорость
            var yVeloctiy = CalculateYVelocity(); //высчитывание скорости физ тела по оси Y
            _rigidbody.velocity = new Vector2(xVelocity, yVeloctiy);
           
            _animator.SetBool(IsGroundKey, _isGrounded);
            _animator.SetFloat(VerticalVelocityKey, _rigidbody.velocity.y);
            _animator.SetBool(IsRunning, _direction.x != 0);

            UpdateSpriteDirection();
        }

        private float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y; //если yVelocity не изменится в процессе, вернется значение по умолчанию
            var isJumpPressing = _direction.y > 0; //если _direction по оси Y больше 0, значит нажата кнопка прыжка 
            
            if (_isGrounded) _isSecondJumpAllowed = true; //если стоим на земле, двойной прыжок разрешен
            
            if (isJumpPressing) //если нажата кнопка прыжка
            {
                yVelocity = CalculateJumpVelocity(yVelocity); // считаем скорость физ тела по оси Y
            } 
            else if (_rigidbody.velocity.y > 0)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }

        private float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.001f; //проверка на падение 
            if (!isFalling) return yVelocity; //если не падаем, то возвращаем скорость физ тела по оси Y по умолчанию
            
            if (_isGrounded) //сюда заходит если персонаж падает.
            {
                yVelocity = _jumpSpeed; //если падает, то скорость по оси Y для физтела равна предыдущему значению + заданная скорость
            } else if (_isSecondJumpAllowed) 
            {
                yVelocity = _jumpSpeed;
                _isSecondJumpAllowed = false;
            }

            return yVelocity;
        }
        
        private void UpdateSpriteDirection()
        {
            if (_direction.x > 0)
            {
                _sprite.flipX = false;
            }

            if (_direction.x < 0)
            {
                _sprite.flipX = true;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = IsGrounded() ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position, 0.3f);
        }

        private bool IsGrounded()
        {
            return _groundCheck.IsTouchingLayer;
        }

    }  
}