// wwwroot/js/todolist.js
window.initSortable = function (listId, dotNetHelper) {
    // Wait a bit to ensure the DOM is fully rendered
    setTimeout(() => {
        const el = document.getElementById(listId);
        if (!el) {
            console.error(`Element with ID '${listId}' not found`);
            return;
        }

        console.log(`Initializing Sortable for element: ${listId}`);

        // Destroy any existing instance
        if (el.sortableInstance) {
            el.sortableInstance.destroy();
        }

        // Create new instance
        const sortable = new Sortable(el, {
            animation: 150,
            ghostClass: 'sortable-ghost',
            dragClass: 'sortable-drag',
            handle: '.drag-handle',
            delay: 100, // Delay to ensure we're not triggering clicks
            delayOnTouchOnly: true, // Delay only on touch devices
            touchStartThreshold: 5, // More tolerant touch handling
            onStart: function (evt) {
                console.log('Drag started');
                document.body.style.cursor = 'grabbing';
            },
            onEnd: function (evt) {
                console.log('Drag ended');
                document.body.style.cursor = '';

                // Get all item IDs in their new order
                const newOrder = Array.from(el.querySelectorAll('.todo-item')).map(
                    item => parseInt(item.getAttribute('data-id'))
                );

                console.log('New order:', newOrder);

                // Send the new order to .NET
                dotNetHelper.invokeMethodAsync('UpdateTodoOrder', newOrder)
                    .catch(error => console.error('Error updating order:', error));
            }
        });

        // Store the instance for future reference
        el.sortableInstance = sortable;

        console.log('Sortable initialization complete');
        return sortable;
    }, 200); // Small delay to ensure DOM is ready
};

// Reinitialize sortable when component updates
window.reinitSortable = function (listId, dotNetHelper) {
    console.log('Reinitializing sortable');
    window.initSortable(listId, dotNetHelper);
};
