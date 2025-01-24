import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TodoContextmenuComponent } from './todo-contextmenu.component';

describe('TodoContextmenuComponent', () => {
  let component: TodoContextmenuComponent;
  let fixture: ComponentFixture<TodoContextmenuComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TodoContextmenuComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TodoContextmenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
