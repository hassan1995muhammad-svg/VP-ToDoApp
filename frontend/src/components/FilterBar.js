import React from 'react';
import './FilterBar.css';

function FilterBar({ activeCount, filter, onFilterChange }) {
    return (
        <div className="filter-bar">
            <span className="item-count">
                {activeCount} {activeCount === 1 ? 'item' : 'items'} left
            </span>
            <div className="filters">
                <button
                    className={filter === 'All' ? 'active' : ''}
                    onClick={() => onFilterChange('All')}
                >
                    All
                </button>
                <button
                    className={filter === 'Active' ? 'active' : ''}
                    onClick={() => onFilterChange('Active')}
                >
                    Active
                </button>
                <button
                    className={filter === 'Completed' ? 'active' : ''}
                    onClick={() => onFilterChange('Completed')}
                >
                    Completed
                </button>
            </div>
        </div>
    );
}

export default FilterBar;