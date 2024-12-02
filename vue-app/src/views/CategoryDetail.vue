<template>
    <div>
      <h1>分类详情: {{ id }}</h1>
      <el-card class="box-card">
        <template #header>
          <div class="card-header">
            <span>{{ tableData.name }}档案</span> <!-- 动态显示分类名称 -->
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
  
          <el-table-column prop="fieldName" label="信息" width="180" />
          <el-table-column prop="fieldValue" label="数值" width="200" />
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
  
        <!-- 传递 id 给 AddUsers 组件 -->
        <AddUsers ref="addCategory" :dialogTitle="dialogTitle" :tableRow="tableRow" :id="id"></AddUsers>
      </el-card>
    </div>
  </template>
  
  
  <script setup>
  import { defineProps, reactive, onMounted, ref,  watch } from 'vue';
  import axios from "@/api/api_config";
  import AddUsers from '@/components/AddDetail.vue';  // 确保路径正确
  import { isNull } from '@/utils/filter';
  import { ElMessage, ElMessageBox } from 'element-plus';
  
  const tableData = reactive({ list: [] });
  const addCategory = ref(null); // 获取子组件实例
  const dialogTitle = ref("");
  const tableRow = ref({});
  
  const props = defineProps({
    id: {
      type: String,
      required: true
    }
  });
  
  // 获取分类的数据
  const getList = () => {
    const categoryId = props.id; // 获取传入的 id
    return axios.get(`/CategoryData/${categoryId}`).then((res) => {
      tableData.list = res.data;
      console.log(res.data);
    });
  };
  
  // 监听 id 的变化，重新加载数据
  watch(() => props.id, (newId) => {
    console.log('Category ID changed:', newId);
    getList(); // id 变化时重新获取数据
  }, { immediate: true }); // immediate: true 让组件初始化时就能调用一次 getList
  
  // 打开分类页
  const handleDialog = (row) => {
    if (isNull(row)) {
      dialogTitle.value = "添加";
      tableRow.value = { id: "", fieldName: "" }; // 初始化为空
    } else {
      dialogTitle.value = "修改信息";
      tableRow.value = JSON.parse(JSON.stringify(row)); // 深拷贝
    }
  
    addCategory.value.dialogCategory(); // 调用子组件的方法
  };
  
  // 删除特定数据
  const open = (categoryDataId) => {
    ElMessageBox.confirm(
      '确定要删除这条数据吗?',
      '你确定要删除这条数据吗?',
      {
        confirmButtonText: '确认',
        cancelButtonText: '取消',
        type: 'warning',
      }
    )
      .then(() => {
        const categoryId = props.id;
        axios.delete(`/CategoryData/${categoryId}/${categoryDataId}`).then(() => {
          ElMessage({
            type: 'success',
            message: '删除成功',
          });
          getList(); // 删除后刷新列表
        });
      })
      .catch(() => {
        ElMessage({
          type: 'info',
          message: '取消删除',
        });
      });
  };
  
  onMounted(() => {
    getList();
  });
  </script>
  
  