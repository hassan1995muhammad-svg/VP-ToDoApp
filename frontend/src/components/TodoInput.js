import React, { useState } from 'react';
import './TodoInput.css';

function TodoInput({ onCreateTask }) {
    const [description, setDescription] = useState('');
    const [deadline, setDeadline] = useState('');
    const [isExpanded, setIsExpanded] = useState(false);

    const handleSubmit = (e) => {
        e.preventDefault();
        if (description.trim().length >= 10) {
            onCreateTask(description, deadline);
            setDescription('');
            setDeadline('');
            setIsExpanded(false);
        } else {
            alert('Task description must be at least 10 characters');
        }
    };

    return (
        <form className="todo-input" onSubmit={handleSubmit}>
            <div className="input-wrapper">
                <span className="chevron" onClick={() => setIsExpanded(!isExpanded)}>
                    {isExpanded ? '▼' : '▶'}
                </span>
                <input
                    type="text"
                    className="task-input"
                    placeholder="What needs to be done?"
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                    maxLength={500}
                    onFocus={() => setIsExpanded(true)}
                />
            </div>
            {isExpanded && (
                <div className="expanded-options">
                    <input
                        type="date"
                        className="date-input"
                        value={deadline}
                        onChange={(e) => setDeadline(e.target.value)}
                        min={new Date().toISOString().split('T')[0]}
                    />
                    <button type="submit" className="add-btn">
                        Add
                    </button>
                </div>
            )}
        </form>
    );
}

export default TodoInput;