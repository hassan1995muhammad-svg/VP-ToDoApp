import React, { useState } from 'react';
import './TodoItem.css';

function TodoItem({ task, onUpdate, onDelete, onToggle }) {
    const [isEditing, setIsEditing] = useState(false);
    const [editText, setEditText] = useState(task.description);
    const [editDeadline, setEditDeadline] = useState(
        task.deadline ? task.deadline.split('T')[0] : ''
    );

    const handleSave = () => {
        if (editText.trim().length >= 10) {
            onUpdate(task.id, {
                description: editText,
                deadline: editDeadline || null
            });
            setIsEditing(false);
        } else {
            alert('Description must be at least 10 characters');
        }
    };

    const handleCancel = () => {
        setEditText(task.description);
        setEditDeadline(task.deadline ? task.deadline.split('T')[0] : '');
        setIsEditing(false);
    };

    const isCompleted = task.status === 2;
    const isOverdue = task.isOverdue && !isCompleted;

    return (
        <li className={`todo-item ${isCompleted ? 'completed' : ''} ${isOverdue ? 'overdue' : ''}`}>
            {isEditing ? (
                <div className="edit-mode">
                    <input
                        type="text"
                        className="edit-input"
                        value={editText}
                        onChange={(e) => setEditText(e.target.value)}
                        maxLength={500}
                        autoFocus
                    />
                    <input
                        type="date"
                        className="edit-date"
                        value={editDeadline}
                        onChange={(e) => setEditDeadline(e.target.value)}
                        min={new Date().toISOString().split('T')[0]}
                    />
                    <div className="edit-actions">
                        <button onClick={handleSave} className="save-btn">✓</button>
                        <button onClick={handleCancel} className="cancel-btn">✕</button>
                    </div>
                </div>
            ) : (
                <div className="view-mode">
                    <input
                        type="checkbox"
                        className="todo-checkbox"
                        checked={isCompleted}
                        onChange={() => !isCompleted && onToggle(task.id)}
                    />
                    <div className="todo-content">
                        <span className="todo-text">{task.description}</span>
                        {task.deadline && (
                            <span className="todo-deadline">
                                📅 {new Date(task.deadline).toLocaleDateString()}
                            </span>
                        )}
                    </div>
                    <div className="todo-actions">
                        <button onClick={() => setIsEditing(true)} className="edit-btn">
                            ✎
                        </button>
                        <button onClick={() => onDelete(task.id)} className="delete-btn">
                            ✕
                        </button>
                    </div>
                </div>
            )}
        </li>
    );
}

export default TodoItem;