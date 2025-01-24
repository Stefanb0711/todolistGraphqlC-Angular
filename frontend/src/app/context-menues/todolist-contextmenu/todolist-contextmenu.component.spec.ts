import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TodolistContextmenuComponent } from './todolist-contextmenu.component';

describe('TodolistContextmenuComponent', () => {
  let component: TodolistContextmenuComponent;
  let fixture: ComponentFixture<TodolistContextmenuComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TodolistContextmenuComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TodolistContextmenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
