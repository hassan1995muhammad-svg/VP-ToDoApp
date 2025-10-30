import React from 'react';
import TodoItem from './TodoItem';
import './TodoList.css';

function TodoList({ tasks, onUpdateTask, onDeleteTask, onToggleComplete }) {
  return (
    <ul className="todo-list">
      {tasks.length === 0 ? (
        <li className="empty-state">No tasks yet. Add one above!</li>
      ) : (
        tasks.map(task => (
          <TodoItem
            key={task.id}
            task={task}
            onUpdate={onUpdateTask}
            onDelete={onDeleteTask}
            onToggle={onToggleComplete}
          />
        ))
      )}
    </ul>
  );
}

export default TodoList;