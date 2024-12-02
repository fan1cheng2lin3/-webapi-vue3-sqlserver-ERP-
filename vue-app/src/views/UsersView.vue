<template>
    <el-card class="box-card">
      <template #header>
        <div class="card-header">
          <span>用户管理</span>
  
          <el-button type="primary" icon="CirclePlus" round @click="handleDialog(null)">添加用户</el-button>
  
        </div>
      </template>
  
  
      <el-table :data="tableData.list" stripe style="width: 100%">
  <!-- 行号列 -->
  <el-table-column label="序号" width="80">
    <template #default="scope">
      {{ scope.$index + 1 }} <!-- 从1开始计数 -->
    </template>
  </el-table-column>
  
  <el-table-column prop="email" label="邮箱" width="180" />
  <el-table-column prop="password" label="密码" width="200" />
  <el-table-column fixed="right" label="操作" width="180">
    <template #default="scope">
      <el-button type="success" size="small" @click="handleDialog(scope.row)">
        修改
      </el-button>
      <el-button type="danger" size="small" @click="open(scope.row.id)">
        删除
      </el-button>
    </template>
  </el-table-column>
  
  
  </el-table>
  
  <AddUsers ref="addCategory"
  :dialogTitle="dialogTitle"
  :tableRow="tableRow"></AddUsers>
    </el-card>
  
  </template>
  
  
  <script setup>
  
  
  import { reactive,onMounted,ref,provide } from 'vue';
  import axios from "@/api/api_config";
  import AddUsers from '@/components/AddUsers.vue';
  import { isNull } from '@/utils/filter';
  import { ElMessage, ElMessageBox } from 'element-plus'
  
  ///----------------------------------
  const tableData = reactive({list:[]})
  const addCategory = ref(null) //获取子属性组件实例
  const dialogTitle = ref("")
  const tableRow = ref({})
  
  
  //-----------------------------------
  
  
  onMounted(()=>{
  getList()
  })
  
  //获取分类的信息
  const getList=()=>{
  return axios.get('/Users').then((res)=>{
    tableData.list = res.data
    console.log(res.data);
  });
  };
  
  provide("getList",getList);
  
  //打开分类页
  const handleDialog = (row) => {
  if (isNull(row)) {
    dialogTitle.value = "添加用户";
    tableRow.value = { id: "", eamil: "" }; // 初始化为空
  } else {
    dialogTitle.value = "修改用户信息";
    tableRow.value = JSON.parse(JSON.stringify(row)); // 深拷贝
  }
  
  addCategory.value.dialogCategory(); // 调用子组件的方法
  };
  
  
  
  const open=(id)=>{
  ElMessageBox.confirm(
    '确定要删除吗?',
    '你确定要删除吗',
    {
      confirmButtonText: '确认',
      cancelButtonText: '取消',
      type: 'warning',
    }
  )
    .then(() => {
      axios.delete(`/Users/${id}`).then(()=>{
        ElMessage({
        type: 'success',
        message: '删除成功',
      });
      getList();
      });
    })
    .catch(() => {
      ElMessage({
        type: 'info',
        message: '取消删除',
      })
    })
  }
  
  </script>