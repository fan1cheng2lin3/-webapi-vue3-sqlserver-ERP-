import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import { getToken, isTokenFromLocalStorageVaild } from '@/auth/auth.service'
import CategoryDetail from '@/views/CategoryDetail.vue'; 

const routes = [
  {
    path: '/login',
    name: 'login',
    component: () => import('@/auth/views/UserLogin.vue')
  },
  {
    path: '/',
    name: '/',
    component: () => import('@/views/LayoutView.vue'),
    redirect: '/home',
    children: [


      {
        path: '/home',
        name: 'home',
        component: HomeView
      },
      {
        path: '/about',
        name: 'about',
        component: () => import( '../views/AboutView.vue')
      },
      {
        path: '/category',
        name: 'category',
        component: () => import('../views/CategoryView.vue')
      },
      {
        path: '/Users',
        name: 'Users',
        component: () => import('../views/UsersView.vue')
      },
      {
        path: '/Roles',
        name: 'Roles',
        component: () => import('../views/RolesView.vue')
      },
      {
        path: '/Permissions',
        name: 'Permissions',
        component: () => import('../views/PermissionsView.vue')
      },
      {
        path: '/category/:id',  // :id 是动态路由参数
        name: 'categoryDetail',
        component: CategoryDetail,
        props: true, // 启用 props 使得路由参数作为 props 传递给组件
      },
      {
        path: '/Product',
        name: 'Product',
        component: () => import('../views/ProductView.vue')
      },
      {
        path: '/Customer',
        name: 'Customer',
        component: () => import('../views/CustomerView.vue')
      },
      {
        path: '/Storehouse',
        name: 'Storehouse',
        component: () => import('../views/StorehouseView.vue')
      },
      {
        path: '/Supplier',
        name: 'Supplier',
        component: () => import('../views/SupplierView.vue')
      },
      {
        path: '/Purchase',
        name: 'Purchase',
        component: () => import('../views/PurchaseView.vue')
      },
      {
        path: '/Textll',
        name: 'Textll',
        component: () => import('../views/TextllView.vue')
      },
      {
        path: '/Warehou',
        name: 'Warehou',
        component: () => import('../views/WarehouseView.vue')
      },
      {
        path: '/Sale',
        name: 'Sale',
        component: () => import('../views/SaleView.vue')
      },
     
    ]
  },

]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})



router.beforeEach((to, from, next) => {

  //to:可以获取到目的地的路由
  //from:可以获取到从哪里来的路由
  //next():放行,(用法1-next（）表示继续、2-next("/1ogin")去哪里、3-next(flase)取消当前导航）

  if (getToken()) {
    // 已经登录，可以添加一些逻辑，比如检查用户权限
    if (to.path === "/login") {
      next("/")
    }
    else {
      if (isTokenFromLocalStorageVaild()) {
        //已经过期
        next("/login")
      }
      else {
        next()
      }

      next()
    }
    next() // 确保总是调用 next()
  } else {
    if (to.path === "/login") {
      next() // 已经是登录页面，无需重定向
    } else {
      next("/login") // 重定向到登录页面
    }
  }
  console.log(333) // 确保在 next() 调用后输出日志
})



export default router