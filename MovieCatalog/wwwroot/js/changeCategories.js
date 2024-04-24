class ChangeCategories {
    constructor() {
        this.selectedCategories = new Set();
        this.categoryList = document.getElementById('categoryList');
        this.categoryModal = new bootstrap.Modal(document.getElementById('categoryModal'), { backdrop: 'static' });
        this.openCategoryModalButton = document.getElementById('openCategoryModal');
        this.addCategoriesButton = document.getElementById('addCategories');

        this.openCategoryModalButton.addEventListener('click', () => this.openCategoryModal());
        this.addCategoriesButton.addEventListener('click', () => this.updateFilmCategories());
    }

    openCategoryModal() {
        fetch('/api/CategoryAPI')
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to fetch categories');
                }
                return response.json();
            })
            .then(categories => {
                this.displayCategories(categories);
                this.getFilmCategoriesAndMarkSelected();
                this.categoryModal.show();
            })
            .catch(error => {
                console.error('Error fetching categories:', error.message);
            });
    }

    displayCategories(categories) {
        this.categoryList.innerHTML = '';
        categories.forEach(category => {
            const listItem = document.createElement('li');
            listItem.textContent = category.name;
            listItem.classList.add('list-group-item');
            listItem.dataset.categoryId = category.id;
            listItem.addEventListener('click', () => this.toggleCategory(listItem));
            if (this.selectedCategories.has(category.id)) { 
                listItem.classList.add('selected');
            }
            this.categoryList.appendChild(listItem);
        });
    }

    getFilmCategoriesAndMarkSelected() {
        const filmId = document.getElementById('Id').value;
        fetch(`/api/CategoryAPI/FilmWithCategories/${filmId}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to fetch film categories');
                }
                return response.json();
            })
            .then(film => {
                film.categories.forEach(category => {
                    this.selectedCategories.add(category.id);
                    const listItem = document.querySelector(`li[data-category-id="${category.id}"]`);
                    if (listItem) {
                        listItem.classList.add('selected');
                    }
                });
            })
            .catch(error => {
                console.error('Error fetching film categories:', error.message);
            });
    }

    toggleCategory(listItem)
    {
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
        fetch(`/api/CategoryAPI/${filmId}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify([...this.selectedCategories])
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
}

window.onload = () => {
    const changeCategories = new ChangeCategories();
};
