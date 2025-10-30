import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './App.css';
import TodoList from './components/TodoList';
import TodoInput from './components/TodoInput';
import FilterBar from './components/FilterBar';

const API_URL = 'http://localhost:5059/api/tasks';

function App() {
    const [tasks, setTasks] = useState([]);
    const [filteredTasks, setFilteredTasks] = useState([]);
    const [filter, setFilter] = useState('All');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetchTasks = async () => {
        try {
            setLoading(true);
            setError(null);
            const response = await axios.get(API_URL);
            setTasks(response.data);
            setFilteredTasks(response.data);
        } catch (err) {
            setError('Failed to load tasks. Make sure backend is running at http://localhost:5059');
            console.error('Error fetching tasks:', err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchTasks();
    }, []);

    useEffect(() => {
        if (filter === 'All') {
            setFilteredTasks(tasks);
        } else if (filter === 'Active') {
            setFilteredTasks(tasks.filter(task => task.status === 1));
        } else if (filter === 'Completed') {
            setFilteredTasks(tasks.filter(task => task.status === 2));
        }
    }, [filter, tasks]);

    const handleCreateTask = async (description, deadline) => {
        try {
            const newTask = { description, deadline: deadline || null };
            const response = await axios.post(API_URL, newTask);
            setTasks([response.data, ...tasks]);
        } catch (err) {
            alert('Failed to create task: ' + (err.response?.data?.message || err.message));
        }
    };

    const handleUpdateTask = async (id, updates) => {
        try {
            const response = await axios.put(`${API_URL}/${id}`, updates);
            setTasks(tasks.map(task => task.id === id ? response.data : task));
        } catch (err) {
            alert('Failed to update task');
        }
    };

    const handleDeleteTask = async (id) => {
        if (window.confirm('Delete this task?')) {
            try {
                await axios.delete(`${API_URL}/${id}`);
                setTasks(tasks.filter(task => task.id !== id));
            } catch (err) {
                alert('Failed to delete task');
            }
        }
    };

    const handleToggleComplete = async (id) => {
        try {
            await axios.patch(`${API_URL}/${id}/complete`);
            fetchTasks();
        } catch (err) {
            alert('Failed to mark as completed');
        }
    };

    const activeCount = tasks.filter(task => task.status === 1).length;

    return (
        <div className="App">
            <div className="container">
                <h1 className="title">todos</h1>
                <div className="todo-card">
                    <TodoInput onCreateTask={handleCreateTask} />
                    {error && <div className="error-message">{error}</div>}
                    {loading ? (
                        <div className="loading">Loading...</div>
                    ) : (
                        <>
                            <TodoList
                                tasks={filteredTasks}
                                onUpdateTask={handleUpdateTask}
                                onDeleteTask={handleDeleteTask}
                                onToggleComplete={handleToggleComplete}
                            />
                            <FilterBar
                                activeCount={activeCount}
                                filter={filter}
                                onFilterChange={setFilter}
                            />
                        </>
                    )}
                </div>
            </div>
        </div>
    );
}

export default App;