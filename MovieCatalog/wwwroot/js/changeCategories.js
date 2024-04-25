class ChangeCategories {
    constructor() {
        this.selectedCategories = new Set();
        this.categoryList = document.getElementById('categoryList');
        this.categoryModal = new bootstrap.Modal(document.getElementById('categoryModal'), { backdrop: 'static' });
        this.openCategoryModalButton = document.getElementById('openCategoryModal');
        this.addCategoriesButton = document.getElementById('addCategories');

        this.openCategoryModalButton.addEventListener('click', () => this.openCategoryModal());
        this.addCategoriesButton.addEventListener('click', () => this.updateFilmCategories());

        this.categoryModalElement.addEventListener('hidden.bs.modal', this.removeEventListeners.bind(this));
    }

    openCategoryModal() {
        this.getCategories()
            .then(categories => {
                this.displayCategories(categories);
                this.getFilmCategoriesAndMarkSelected();
                this.categoryModal.show();
            })
            .catch(error => {
                console.error('Error fetching categories:', error.message);
            });
    }

    getCategories() {
        return fetch('/api/CategoryAPI')
            .then(response => {
               if (!response.ok) {
                   throw new Error('Failed to fetch categories');
               }
               return response.json();
            });
    }

    displayCategories(categories) {
        this.categoryList.innerHTML = '';

        const categoryNodes = categories.map((category) => this.createListItem(category));
        this.categoryList.append(...categoryNodes);
    }

    createListItem(category) {
        const listItem = document.createElement('li');
        listItem.textContent = category.name;
        listItem.classList.add('list-group-item');
        listItem.dataset.categoryId = category.id;
        listItem.addEventListener('click', () => this.toggleCategory(listItem));
        if (this.selectedCategories.has(category.id)) {
            listItem.classList.add('selected');
        }
        return listItem;
    }

    getFilmCategoriesAndMarkSelected() {
        const filmId = document.getElementById('Id').value;
        this.getFilmCategories(filmId)
            .then(film => {
                film.categories.forEach(category => {
                    this.markSelectedCategory(category.id);
                });
            })
            .catch(error => {
                console.error('Error fetching film categories:', error.message);
            });
    }

    getFilmCategories(filmId) {
        return fetch(`/api/CategoryAPI/FilmWithCategories/${filmId}`)
            .then(response => {
               if (!response.ok) {
                   throw new Error('Failed to fetch film categories');
               }
               return response.json();
            });
    }

    markSelectedCategory(categoryId) {
        const listItem = document.querySelector(`li[data-category-id="${categoryId}"]`);
        if (listItem) {
            listItem.classList.add('selected');
        }
        this.selectedCategories.add(categoryId);
    }

    toggleCategory(listItem) {
        const categoryId = parseInt(listItem.dataset.categoryId);
        if (this.selectedCategories.has(categoryId)) {
            this.selectedCategories.delete(categoryId);
            listItem.classList.remove('selected');
        } else {
            this.selectedCategories.add(categoryId);
            listItem.classList.add('selected');
        }
    }

    updateFilmCategories() {
        const filmId = document.getElementById('Id').value;
        const requestBody = {
            filmId: filmId,
            newCategories: [...this.selectedCategories]
        };

        fetch(`/api/CategoryAPI`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(requestBody)
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to add categories');
                }
                console.log('Selected categories added successfully');
                this.categoryModal.hide();
            })
            .catch(error => {
                console.error('Error adding categories:', error.message);
            });
    }

    removeEventListeners() {
        this.openCategoryModalButton.removeEventListener('click', this.openCategoryModalHandler);
        this.addCategoriesButton.removeEventListener('click', this.updateFilmCategoriesHandler);
    }
}

window.onload = () => {
    const changeCategories = new ChangeCategories();
};
